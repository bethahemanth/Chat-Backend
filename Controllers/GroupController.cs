using Microsoft.AspNetCore.Mvc;
using ChatApplication.Services;
using ChatAppBackend.Models;
[ApiController]
[Route("api/groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpDelete("members/{group_id}")]
    public string DeleteGroup(int group_id)
    {
        return _groupService.DeleteGroup(group_id);
    }

    [HttpGet("members/{group_id}")]
    public IActionResult GetAllMembers(int group_id)
    {
        var members = _groupService.GetAllGroupMembers(group_id);
        return Ok(members);
    }

    [HttpGet("groupid/{user_id}")]
    public IActionResult GetGroupIdByMember(int user_id)
    {
        var groupId = _groupService.GetGroupIdByMemberId(user_id);
        return Ok(groupId);
    }

    [HttpPost]
    public string InsertGroup([FromBody] ChatGroup group)
    {
        if (group == null)
        {
            return "Invalid group data.";  // Return error message if the input is null
        }

        // Call the InsertGroup method of GroupService to insert the group
        return _groupService.InsertGroup(group.owner_id, group.group_name, group.created_at);
    }
    [HttpGet("groupname/{group_id}")]
    public IActionResult GetGroupName(int group_id)
    {
        var groupName = _groupService.GetGroupNameByGroupId(group_id);
        return Ok(groupName);
    }

    [HttpGet("owner/{group_id}")]
    public IActionResult GetGroupOwner(int group_id)
    {
        var ownerId = _groupService.GroupOwnerByGroupId(group_id);
        return Ok(ownerId);
    }

    [HttpGet("groupsbyowner/{owner_id}")]
    public IActionResult GetGroupsByOwner(int owner_id)
    {
        var groups = _groupService.GetGroupByOwnerId(owner_id);
        return Ok(groups);
    }

}