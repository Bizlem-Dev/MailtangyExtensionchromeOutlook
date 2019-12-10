namespace MailTangy
{
    public class CreateTaskApiResponse
    {
        public string Message { get; set; }
        public string OwnerId { get; set; }
        public string TaskId { get; set; }
    }

    /*
     * OUTPUT:-
        {
          "message" : "Task created successfully",
          "ownerId" : "0056F00000AiGYnQAN",
          "TaskId" : "00T6F0000521UoRUAU"
        }
     */
}
