using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppWebAPI.Data;
using MusicAppWebAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Users/{username}/credentials
    [HttpGet("{username}/credentials")]
    public async Task<ActionResult<(string HashedPassword, string Salt)>> GetCredentials(string username)
    {
        var user = await _context.Users
            .Where(u => u.Username == username)
            .Select(u => new { u.HashedPassword, u.Salt })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        return Ok((user.HashedPassword, user.Salt));
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> RegisterUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
    }

    // PUT: api/Users/{username}/begin-session
    [HttpPut("{username}/begin-session")]
    public async Task<IActionResult> BeginSession(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/Users/{username}/end-session
    [HttpPut("{username}/end-session")]
    public async Task<IActionResult> EndSession(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }
}

