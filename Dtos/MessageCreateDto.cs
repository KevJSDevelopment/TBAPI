namespace TwitterBattlesAPI.Dtos
{
    public class MessageCreateDto
    {
        public int UserWhoMessagedId { get; set; }

        public int UserReceivingMessageId { get; set; }

        public string MessageContent { get; set; }
    }
}