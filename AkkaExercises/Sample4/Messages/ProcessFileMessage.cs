namespace AkkaExercises.Sample4.Messages
{
    internal class ProcessFileMessage
    {
        public string FileName { get; private set; }

        public ProcessFileMessage(string fileName)
        {
            FileName = fileName;
        }
    }
}