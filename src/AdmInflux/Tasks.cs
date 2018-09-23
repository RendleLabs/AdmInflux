using System.Threading.Tasks;

namespace AdmInflux
{
    public static class Tasks
    {
        public async static Task<(T1, T2)> Multi<T1, T2>(Task<T1> task1, Task<T2> task2)
        {
            var results = await Task.WhenAll(Cast(task1), Cast(task2));
            return ((T1) results[0], (T2) results[1]);
        }

        private static async Task<object> Cast<T>(Task<T> task) 
        {
            object o = await task.ConfigureAwait(false);
            return o;
        }
    }
}