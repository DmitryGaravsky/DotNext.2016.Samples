namespace ConsoleApplication1 {
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program {
        interface IDataSource<T> {
            int GetCount();
            T GetValueByIndex(int index);
            void OrderByValue(); // Optional Implementation
        }
        class DataSource : IDataSource<int> {
            int[] array;
            public DataSource(int[] array) {
                this.array = array;
            }
            public int GetCount() {
                Thread.Sleep(250);
                Console.WriteLine("COUNT");
                return array.Length;
            }
            public int GetValueByIndex(int index) {
                Thread.Sleep(100);
                Console.WriteLine("[" + index.ToString() + "]");
                return array[index];
            }
            public void OrderByValue() {
                Thread.Sleep(500);
                throw new NotSupportedException("ORDERBY");
            }
        }
        //
        static void Main(string[] args) {
            Func<IDataSource<int>, int> sumEven = (dataSource) =>
            {
                int count = dataSource.GetCount();
                int sum = 0;
                for(int i = 0; i < count; i++) {
                    var value = dataSource.GetValueByIndex(i);
                    if(value % 2 == 0)
                        sum += value;
                }
                return sum;
            };
            //
            Func<IDataSource<int>, int> sumOdd = (dataSource) =>
            {
                int count = dataSource.GetCount();
                int sum = 0;
                for(int i = 0; i < count; i++) {
                    var value = dataSource.GetValueByIndex(i);
                    if(value % 2 != 0)
                        sum += value;
                }
                return sum;
            };
            //
            var ds = new DataSource(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Proceed(ds, (dataSource) =>
            {
                int count = dataSource.GetCount();
                dataSource.OrderByValue();
                int se = sumEven(dataSource);
                int so = sumOdd(dataSource);
                return (se + so) / count;
                //var tCount = Task.Run(() => dataSource.GetCount());
                //var tSE = Task.Run(() => sumEven(dataSource));
                //var tSO = Task.Run(() => sumOdd(dataSource));
                //Task.WaitAny(tCount, tSE, tSO);
                //return (tSE.Result + tSO.Result) / tCount.Result;
            });
        }
        //
        static void Proceed(DataSource dataSource, Func<DataSource, int> scenario) {
            var start = DateTime.Now;
            System.Console.WriteLine("Result=" + scenario(dataSource).ToString());
            System.Console.WriteLine("Elapsed(ms)=" + (DateTime.Now - start).TotalSeconds.ToString("F1"));
        }
    }
}