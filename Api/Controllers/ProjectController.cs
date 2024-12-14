using Microsoft.AspNetCore.Mvc;

using Api.Database;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ProjectContext _projectContext;

        public ProjectController(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        [HttpGet(nameof(GetProjects))]
        public async Task<ActionResult<List<Project>>> GetProjects()
        {
            return await _projectContext.Projects.ToListAsync();
        }

        [HttpPost(nameof(CreateProject))]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO projectDto)
        {
            var project = new Project()
            {
                Id = Guid.NewGuid(),
                Name = projectDto.Name,
                StartTime = projectDto.StartTime,
                EndTime = projectDto.EndTime,
                TimeSpent = projectDto.TimeSpent ?? TimeSpan.Zero,
                IsCompleted = false
            };

            _projectContext.Projects.Add(project);
            await _projectContext.SaveChangesAsync();

            return Ok(project);
        }

        [HttpPost($"{nameof(LogTime)}/{{projectId}}")]
        public async Task<IActionResult> LogTime(Guid projectId, [FromBody] TimeLogDTO timeLogDto)
        {
            var project = await _projectContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
            if (project is null)
            {
                return BadRequest("Specified project doesn't exist");
            }

            if (project.IsCompleted)
            {
                return BadRequest("Cannot add time to a completed project");
            }

            var log = new TimeLog()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                StartTime = timeLogDto.StartTime,
                EndTime = timeLogDto.EndTime,
                TimeStamp = DateTimeOffset.UtcNow
            };

            project.TimeSpent += log.EndTime - log.StartTime;

            _projectContext.TimeLogs.Add(log);

            await _projectContext.SaveChangesAsync();

            return Ok(project);
        }

        [HttpPost($"{nameof(CompleteProject)}/{{projectId}}")]
        public async Task<IActionResult> CompleteProject(Guid projectId)
        {
            var project = await _projectContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
            if (project is null)
            {
                return BadRequest("Specified project doesn't exist");
            }

            project.IsCompleted = true;

            await _projectContext.SaveChangesAsync();

            return Ok(project);
        }

        [HttpDelete($"{nameof(DeleteProject)}/{{projectId}}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var project = await _projectContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
            if (project is null)
            {
                return BadRequest("Specified project doesn't exist");
            }

            var relatedTimeLogs = await _projectContext.TimeLogs.Where(tl => tl.ProjectId == projectId).ToListAsync();

            _projectContext.TimeLogs.RemoveRange(relatedTimeLogs);
            _projectContext.Projects.Remove(project);
            await _projectContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet($"{nameof(GetProjectTimeLogs)}/{{projectId}}")]
        public async Task<ActionResult<List<TimeLog>>> GetProjectTimeLogs(Guid projectId)
        {
            var project = await _projectContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
            if (project is null)
            {
                return BadRequest("Specified project doesn't exist");
            }

            var relatedTimeLogs = await _projectContext.TimeLogs.Where(tl => tl.ProjectId == projectId).ToListAsync();

            var mergedTimeLogs = MergeTimeLogs(relatedTimeLogs);

            return Ok(mergedTimeLogs);
        }

        private List<TimeLog>? MergeTimeLogs(List<TimeLog> timeLogs)
        {
            if (timeLogs == null || timeLogs.Count < 2) return timeLogs;

            timeLogs = timeLogs.OrderBy(t => t.StartTime).ToList();

            for (int i = 0; i < timeLogs.Count - 1; i++)
            {
                var currentLog = timeLogs[i];
                var nextLog = timeLogs[i + 1];

                if (currentLog.EndTime > nextLog.StartTime)
                {
                    var mergedLog = new TimeLog
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = currentLog.ProjectId,
                        TimeStamp = DateTimeOffset.Now,
                        StartTime = currentLog.StartTime < nextLog.StartTime ? currentLog.StartTime : nextLog.StartTime,
                        EndTime = currentLog.EndTime > nextLog.EndTime ? currentLog.EndTime : nextLog.EndTime
                    };

                    timeLogs[i] = mergedLog;
                    timeLogs.RemoveAt(i + 1);
                    i--;
                }
            }

            return timeLogs;
        }
    }
}
