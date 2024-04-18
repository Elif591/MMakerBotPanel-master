namespace MMakerBotPanel.Business
{
    using MMakerBotPanel.Database.Context;
    using System;

    public class WorkerHelper
    {
        public static string CreateWorkerKey()
        {
            _ = new ModelContext();
            for (; ; )
            {
                string workerKey = CreateKey();
                //Worker worker = db.Workers.FirstOrDefault(x => x.WorkerKey == workerKey);
                //if (worker == null)
                //{
                return workerKey;
                //}
            }

        }

        private static string CreateKey()
        {
            Random rd = new Random();
            string Key = "";
            for (int i = 0; i < 25; i++)
            {
                int a = rd.Next(0, 9);
                Key += a.ToString();
            }
            return Key;
        }
    }
}