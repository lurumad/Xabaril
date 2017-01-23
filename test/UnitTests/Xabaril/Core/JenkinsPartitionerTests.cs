using global::Xabaril.Core.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Xabaril.Core
{
    

    public class jenkins_partitioner_should
    {
        const int PARTITION_COUNTS = 10;

        [Fact]
        public void get_allways_the_same_partition_for_same_data()
        {
            var data = "some_string_data";

            var partition1 = JenkinsPartitioner.ResolveToLogicalPartition(data, PARTITION_COUNTS);
            var partition2 = JenkinsPartitioner.ResolveToLogicalPartition(data, PARTITION_COUNTS);

            Assert.Equal(partition1, partition2);
        }

        [Fact]
        public void shard_data()
        {
            var partitions = new Dictionary<int, int>();

            foreach (var item in _users)
            {
                var key = JenkinsPartitioner.ResolveToLogicalPartition(item, PARTITION_COUNTS);

                if (!partitions.ContainsKey(key))
                {
                    partitions.Add(key, 1);
                }
                else
                {
                    partitions[key] = ++partitions[key];
                }
            }


            var values = partitions.Values;
            var average = values.Average();

            var stdev = Math.Sqrt(values.Select(v => Math.Pow(v - average, 2)).Sum() / PARTITION_COUNTS);

            Assert.True(values.Count == 10);
            Assert.True(stdev < 2);
        }


        List<string> _users = new List<string>()
        {
            "uzorrilla",
            "lruiz",
            "mrcabello",
            "lfraile",
            "axelgorris",
            "gerardorfuentes",
            "davidcandia",
            "fancibon93",
            "zimzumac",
            "danielfcea",
            "crahum",
            "pedrogovi",
            "dannywalls",
            "ncdeza",
            "luisnet9",
            "toniogalo",
            "tjMicrosoft",
            "EmilioLigero",
            "mario_r21",
            "_camaya",
            "manutsss",
            "tori_parla",
            "Alessandro__G",
            "radu_popovici",
            "franantares",
            "jjane90",
            "VictorVelarde",
            "elcurto84",
            "juanma_nieves",
            "JosePampas",
            "edufornet",
            "thuweb",
            "fernanSQL",
            "samuelchicon",
            "arturoferreiro",
            "carmalino",
            "davidGooomez",
            "mlean_labs",
            "bjabinn",
            "lestaPkr",
            "vicpada",
            "GalletasMaria",
            "Yale_llamaremos",
            "jserran0",
            "serrano5510",
            "ibonilm",
            "quiqu3",
            "eiximenis",
            "rcorral",
            "etomas",
        };
    }
}
