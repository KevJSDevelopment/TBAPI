namespace TwitterBattlesAPI.Dtos
{
    public class FollowerCreateDto
    {
        public int UserThatFollowedId { get; set; }

        public int UserBeingFollowedId { get; set; }
    }
}