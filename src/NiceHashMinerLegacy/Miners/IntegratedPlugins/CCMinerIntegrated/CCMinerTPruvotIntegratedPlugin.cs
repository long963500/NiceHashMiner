﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinerPluginToolkitV1;
using NiceHashMinerLegacy.Common.Algorithm;
using NiceHashMinerLegacy.Common.Device;
using NiceHashMinerLegacy.Common.Enums;

namespace NiceHashMiner.Miners.IntegratedPlugins
{
    class CCMinerTpruvotIntegratedPlugin : CCMinersPluginBase
    {
        public override string PluginUUID => "CCMinerTpruvot";

        public override Version Version => new Version(1,0);

        public override string Name => "CCMinerTpruvot";

        protected override string DirPath => "ccminer_tpruvot";

        public override Dictionary<BaseDevice, IReadOnlyList<Algorithm>> GetSupportedAlgorithms(IEnumerable<BaseDevice> devices)
        {
            var supported = new Dictionary<BaseDevice, IReadOnlyList<Algorithm>>();
            var reqCudaVer = Checkers.CudaVersion.CUDA_10_0_130;
            var isCompatible = Checkers.IsCudaCompatibleDriver(reqCudaVer, CUDADevice.INSTALLED_NVIDIA_DRIVERS);
            if (!isCompatible) return supported; // return emtpy

            var cudaGpus = devices
                .Where(dev => dev is CUDADevice gpu && gpu.SM_major >= 3)
                .Cast<CUDADevice>();

            foreach (var gpu in cudaGpus)
            {
                var algorithms = new List<Algorithm> {
                    new Algorithm(PluginUUID, AlgorithmType.NeoScrypt),
                    new Algorithm(PluginUUID, AlgorithmType.Blake2s),
                    new Algorithm(PluginUUID, AlgorithmType.Keccak),
                    new Algorithm(PluginUUID, AlgorithmType.Skunk),
                    new Algorithm(PluginUUID, AlgorithmType.Lyra2REv3),
                    new Algorithm(PluginUUID, AlgorithmType.X16R), // TODO check performance
                };
                supported.Add(gpu, algorithms);
            }

            return supported;
        }
    }
}