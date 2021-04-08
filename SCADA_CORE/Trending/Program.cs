using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Trending
{
    public class CallbackClass : TrendingServiceReference.ITrendingServiceCallback
    {
        public void TagValueChangedMethod(string tagId, double value)
        {
            Console.WriteLine($"{DateTime.Now} | Tag name: {tagId} | value : {value}");
        }
    }

    class Program
    {
        static TrendingServiceReference.TrendingServiceClient client = null;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new CallbackClass());
            client = new TrendingServiceReference.TrendingServiceClient(ic);

            client.Init();

            while (true) ;
        }
    }
}
