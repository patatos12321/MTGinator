using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace MTGinator
{
    public class Program
    {
        private static X509Certificate2 _cert;

        private static X509Certificate2 GetSelfSignedCertificate()
        {
            if (_cert != null)
                return _cert;

            var distinguishedName = new X500DistinguishedName($"CN=MTGinatorDev");

            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

                // Generate semi random password
                var password = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                _cert = new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.MachineKeySet);
            }

            return _cert;
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //If you want to test the SelfSignedCertificate, run in release
#if DEBUG
                
#else
            .UseKestrel(kestrelOptions =>
                {
                    kestrelOptions.ConfigureHttpsDefaults(listenOptions =>
                    {
                        listenOptions.ServerCertificate = GetSelfSignedCertificate();
                    });
                })
#endif
                .UseStartup<Startup>();
    }
}
