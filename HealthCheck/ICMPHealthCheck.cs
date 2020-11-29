using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
    public class ICMPHealthCheck : IHealthCheck
    {
        private string Host { get;set;}
        private int Timeout { get;set;}

        public ICMPHealthCheck(string host, int timeout)
        {
            Host = host;
            Timeout = timeout;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                { 
                    var reply = await ping.SendPingAsync(Host);

                    switch (reply.Status)
                    {
                        case IPStatus.Success:
                            var msg = String.Format(
                                    "IMCP to {0} took {1} ms.", Host, reply.RoundtripTime);
                            return (reply.RoundtripTime > Timeout) ? HealthCheckResult.Degraded(msg) : HealthCheckResult.Healthy(msg);
                             
                        //case IPStatus.DestinationNetworkUnreachable:
                        //    break;
                        //case IPStatus.DestinationHostUnreachable:
                        //    break;
                        //case IPStatus.DestinationProhibited:
                        //    break; 
                        //case IPStatus.DestinationPortUnreachable:
                        //    break;
                        //case IPStatus.NoResources:
                        //    break;
                        //case IPStatus.BadOption:
                        //    break;
                        //case IPStatus.HardwareError:
                        //    break;
                        //case IPStatus.PacketTooBig:
                        //    break;
                        //case IPStatus.TimedOut:
                        //    break;
                        //case IPStatus.BadRoute:
                        //    break;
                        //case IPStatus.TtlExpired:
                        //    break;
                        //case IPStatus.TtlReassemblyTimeExceeded:
                        //    break;
                        //case IPStatus.ParameterProblem:
                        //    break;
                        //case IPStatus.SourceQuench:
                        //    break;
                        //case IPStatus.BadDestination:
                        //    break;
                        //case IPStatus.DestinationUnreachable:
                        //    break;
                        //case IPStatus.TimeExceeded:
                        //    break;
                        //case IPStatus.BadHeader:
                        //    break;
                        //case IPStatus.UnrecognizedNextHeader:
                        //    break;
                        //case IPStatus.IcmpError:
                        //    break;
                        //case IPStatus.DestinationScopeMismatch:
                        //    break;
                        default:
                            var err = String.Format("IMCP to {0} failed: {1} ", Host, reply.Status);
                            return HealthCheckResult.Unhealthy(err);
                            
                            
                    }
                }
            }
            catch (Exception ex)
            {
                var err = String.Format("ICMP to {0} failed:{1}", Host, ex.Message);
                return HealthCheckResult.Unhealthy(err);
            }
        }
    }
}
