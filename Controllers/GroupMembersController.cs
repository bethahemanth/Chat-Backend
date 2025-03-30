using ChatAppBackend.Models;
using ChatApplication.Services.Service_Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
    {
        [ApiController]
        [Route("api/groupmembers")]
    public class GroupMembersController : ControllerBase
    {
        private readonly IGroupMembers _groupMembersService;

        // Dependency Injection of the IGroupMembers service
        public GroupMembersController(IGroupMembers groupMembersService)
        {
            _groupMembersService = groupMembersService;
        }

        // POST api/groupmembers/insert
        [HttpPost("insert")]
        public ActionResult<string> InsertMember([FromBody] GroupMember groupMember)
        {
            if (groupMember == null)
            {
                return BadRequest("Invalid group member data.");
            }

            var result = _groupMembersService.InsertMember(groupMember);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to insert group member.");
            }

            return Ok("Group member inserted successfully.");
        }

        // DELETE api/groupmembers/delete/{group_member_id}
        [HttpDelete("delete/{user_id}")]
        public ActionResult<string> DeleteMember(int user_id)
        {
            var result = _groupMembersService.DeleteMember(user_id);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to delete group member.");
            }

            return Ok("Group member deleted successfully.");
        }

        // GET api/groupmembers/{group_member_id}
        [HttpGet("{user_id}")]
        public ActionResult<GroupMember> GetMember(int user_id)
        {
            var member = _groupMembersService.GetMember(user_id);
            if (member == null)
            {
                return NotFound("Group member not found.");
            }

            return Ok(member);
        }
    }
    }

