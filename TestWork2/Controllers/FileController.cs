using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWork2.Data;
using TestWork2.Data.Models;
using TestWork2.Extensions;
using File = TestWork2.Data.Models.File;
using FileResult = TestWork2.Data.Models.FileResult;

namespace TestWork2.Controllers;

[ApiController]
[Route("files")]
public class FileController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IValidator<File> _fileValidator;
    private readonly IValidator<FileValue> _fileValueValidator;

    public FileController(AppDbContext db, IValidator<File> fileValidator, IValidator<FileValue> fileValueValidator)
    {
        _db = db;
        _fileValidator = fileValidator;
        _fileValueValidator = fileValueValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFile(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var rows = (await reader.ReadToEndAsync()).Split("\r\n");
        if (rows.Length == 0)
            return BadRequest(new { error = "File is empty" });

        var fileModel = await _db.Files.FirstOrDefaultAsync(modelFile =>
            modelFile.Name == Path.GetFileNameWithoutExtension(file.FileName).ToLower());
        if (fileModel == null)
        {
            fileModel = new File()
            {
                Name = Path.GetFileNameWithoutExtension(file.FileName).ToLower(),
                RowCount = rows.Length
            };
            var validationResult = await _fileValidator.ValidateAsync(fileModel);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            await _db.Files.AddAsync(fileModel);
            await _db.SaveChangesAsync();
        }
        else
        {
            await _db.FileValues.Where(fileValue => fileValue.File.Name == fileModel.Name).ExecuteDeleteAsync();
            await _db.FileResults.Where(fileResult => fileResult.File.Name == fileModel.Name).ExecuteDeleteAsync();
        }

        var fileValues = rows.AsParallel().Select(row =>
        {
            var rawFileValue = row.Split(';');
            if (rawFileValue.Length != 3) 
                return null;
            try
            {
                var fileValueModel = new FileValue()
                {
                    DateTime = DateTime.ParseExact(rawFileValue[0], "yyyy-MM-dd_HH-mm-ss",
                        System.Globalization.CultureInfo.InvariantCulture),
                    Seconds = Convert.ToInt32(rawFileValue[1]),
                    Indicator = Convert.ToDouble(rawFileValue[2]),
                    File = fileModel
                };
                var validateResult = _fileValueValidator.Validate(fileValueModel);
                return validateResult.IsValid ? fileValueModel : null;
            }
            catch (Exception exception)
            {
                if (exception is InvalidCastException | exception is FormatException)
                    return null;
                throw new Exception("Unhandled exception", exception);
            }
        }).Where(fileValue => fileValue != null).ToList();

        if (fileValues.Count == 0)
            return BadRequest(new { error = "Bad data in file" });
        
        var minDateTime = fileValues.Min(value => value.DateTime);
        var fileResult = new FileResult()
        {
            MinDateTime = minDateTime,
            ElapsedTime = fileValues.Max(value => value.DateTime) - minDateTime,
            AverageSeconds = fileValues.Average(value => value.Seconds),
            MedianSeconds = fileValues.Median(value => value.Seconds),
            AverageIndicator = fileValues.Average(value => value.Indicator),
            MaxIndicator = fileValues.Max(value => value.Indicator),
            MinIndicator = fileValues.Min(value => value.Indicator),
            RowCount = fileValues.Count,
            File = fileModel
        };

        await _db.FileValues.AddRangeAsync(fileValues); 
        await _db.FileResults.AddAsync(fileResult);
        await _db.SaveChangesAsync();

        var serializedFileValues = fileValues.AsParallel().Select(fileValue => new
        {
            dateTime = fileValue.DateTime,
            seconds = fileValue.Seconds,
            indicator = fileValue.Indicator
        }).ToList();
        return Ok(new
        {
            file = new
            {
                name = fileModel.Name,
                valuesCount = rows.Length,
                validValuesCount = serializedFileValues.Count
            },
            result = new
            {
                minDateTime,
                elapsedTime = fileResult.ElapsedTime,
                averageSeconds = fileResult.AverageSeconds,
                medianSeconds = fileResult.MedianSeconds,
                averageIndicator = fileResult.AverageIndicator,
                maxIndicator = fileResult.MaxIndicator,
                minIndicator = fileResult.MinIndicator,
            },
            values = serializedFileValues
        });
    }

    [HttpGet]
    public IActionResult GetFilesResults([FromQuery] string[]? names, [FromQuery] DateTime? fromDateTime,
        [FromQuery] DateTime? toDateTime, [FromQuery] double? toAverageSeconds,
        [FromQuery] double? toAverageIndicator, [FromQuery] double? fromAverageSeconds = 0,
        [FromQuery] double? fromAverageIndicator = 0)
    {
        toAverageIndicator = toAverageIndicator < 0 ? 0 : toAverageIndicator;
        toAverageSeconds = toAverageSeconds < 0 ? 0 : toAverageSeconds;
        toDateTime = toDateTime == null | toDateTime > DateTime.Now ? DateTime.Now : toDateTime;

        var query = _db.FileResults.Include(fileResult => fileResult.File)
            .Where(fileResult => fileResult.AverageIndicator >= fromAverageIndicator)
            .Where(fileResult => fileResult.AverageSeconds >= fromAverageSeconds)
            .Where(fileResult => fileResult.MinDateTime <= toDateTime);
        foreach (var name in names)
            query = query.Where(fileResult => fileResult.File.Name.Contains(name.ToLower()));
        if (toAverageIndicator != null)
            query = query.Where(fileResult => fileResult.AverageIndicator <= toAverageIndicator);
        if (toAverageSeconds != null)
            query = query.Where(fileResult => fileResult.AverageSeconds <= toAverageSeconds);
        if (fromDateTime != null)
            query = query.Where(fileResult => fileResult.MinDateTime >= fromDateTime);
        
        return Ok(query.ToList());
    }

    [HttpGet("values")]
    public IActionResult GetFilesValues([FromQuery] string[]? names)
    {
        var query = _db.FileValues.AsNoTracking();

        if (names.Length == 0)
            return Ok(query.ToList());

        query = query.Include(fileValue => fileValue.File);
        foreach (var name in names)
            query = query.Where(fileValue => fileValue.File.Name.Contains(name.ToLower()));

        return Ok(query.ToList());
    }
}