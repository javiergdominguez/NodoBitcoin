using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

class BitcoinSeedAnalyzer
{
    static void Main()
    {
        // 🌐 Lista completa de DNS Seeds oficiales para SIGNET extraídas de Bitcoin Core
        string[] signetSeeds = new string[]
        {
            "seed.signet.bitcoin.sprovoost.nl.",
            "seed.signet.achownodes.xyz.",
            "seed.signet.wiz.biz"
        };

        Console.WriteLine("Searching... Consultando todos los nodos semilla oficiales de Signet...");
        Console.WriteLine("====================================================================");

        Dictionary<string, int> conteoIps = new Dictionary<string, int>();
        int totalIpv4 = 0;
        int totalIpv6 = 0;

        foreach (string seed in signetSeeds)
        {
            try
            {
                Console.WriteLine($"\n🌐 Consultando: {seed}");
                IPAddress[] ips = Dns.GetHostAddresses(seed);

                foreach (IPAddress ip in ips)
                {
                    string ipString = ip.ToString();

                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        totalIpv4++;
                        Console.WriteLine($"   -> [IPv4] {ipString}");
                    }
                    else if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        totalIpv6++;
                        Console.WriteLine($"   -> [IPv6] {ipString}");
                    }

                    if (conteoIps.ContainsKey(ipString))
                    {
                        conteoIps[ipString]++;
                    }
                    else
                    {
                        conteoIps[ipString] = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ No se pudo obtener respuesta de esta semilla: {ex.Message}");
            }
        }

        int totalIpsRecibidas = totalIpv4 + totalIpv6;
        int ipsUnicas = 0;
        int ipsRepetidas = 0;
        int totalAparicionesDuplicadas = 0;

        foreach (var registro in conteoIps)
        {
            if (registro.Value == 1)
            {
                ipsUnicas++;
            }
            else
            {
                ipsRepetidas++;
                totalAparicionesDuplicadas += (registro.Value - 1);
            }
        }

        Console.WriteLine("\n====================================================================");
        Console.WriteLine("📊 REPORTE GLOBAL DE ANÁLISIS DE RED (SIGNET)");
        Console.WriteLine("====================================================================");
        Console.WriteLine($"Total de respuestas procesadas: {totalIpsRecibidas}");
        Console.WriteLine($"  ├─ Direcciones IPv4 encontradas: {totalIpv4}");
        Console.WriteLine($"  └─ Direcciones IPv6 encontradas: {totalIpv6}");
        Console.WriteLine();
        Console.WriteLine($"Direcciones IP únicas descubiertas:     {ipsUnicas}");
        Console.WriteLine($"Direcciones IP duplicadas entre seeds:  {ipsRepetidas}");
        Console.WriteLine($"Total de registros redundantes filtrados: {totalAparicionesDuplicadas}");
        Console.WriteLine("====================================================================");

        // 🟢 Línea solicitada para evitar el cierre repentino de la ventana
        Console.WriteLine("\n⌨️  Presiona cualquier tecla para cerrar la consola...");
        Console.ReadKey();
    }
}
