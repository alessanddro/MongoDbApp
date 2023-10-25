using Microsoft.AspNetCore.Mvc;
using MongoDbApp.Api.Models;
using MongoDbApp.Api.Services;

namespace MongoDbApp.Api.Controllers;

[ApiController]
[Route("api/book")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;


    public BooksController(BooksService booksService)
    {
        _booksService = booksService;
    }


    [HttpGet]
    public async Task<List<Book>> Get()
    {
        List<Book> book = await _booksService.GetAsync();
        foreach (var item in book)
        {
            for (int i = 0; i < item.BookCover.Count; i++)
            {
                if (item.BookCover[i].flag == true)
                {
                    //Console.WriteLine(item.BookCover[i].flag);
                    item.BookCover[i].cover = "https://mongoloide.azurewebsites.net/" + item.BookCover[i].cover.Replace("wwwroot/", "");
                }
                else
                {
                    item.BookCover.RemoveAt(i);
                }

            }
        }
        return book;
    }


    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        Book book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }
        for (int i = 0; i < book.BookCover.Count; i++)
        {
            if (book.BookCover[i].flag == true)
            {
                //Console.WriteLine(item.BookCover[i].flag);
                book.BookCover[i].cover = "https://mongoloide.azurewebsites.net/" + book.BookCover[i].cover.Replace("wwwroot/", "");
            }
            else
            {
                book.BookCover.RemoveAt(i);
            }
        }
        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] Book newBook, IFormFile[] cover)
    {
        Cover newCover = new Cover();
        for (int i = 0; i < cover.Length; i++)
        {
            var ext = Path.GetExtension(cover[i].FileName);
            string uuid = Guid.NewGuid().ToString();
            string filePath = Path.Combine("wwwroot/Storage/BookCovers/", uuid + ext);

            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            cover[i].CopyTo(fileStream);
            newCover.cover = filePath;
            newBook?.BookCover?.Add(newCover);
        }
        await _booksService.CreateAsync(newBook);
        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, [FromBody] Book updatedBook)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _booksService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpGet("generateQRCode/{id:length(24)}")]
    public Task<IActionResult> GenerateQRCode(string id)
    {
        return Task.FromResult<IActionResult>(Ok(_booksService.GenerateQRCode(id)));
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _booksService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _booksService.RemoveAsync(id);

        return NoContent();
    }

}