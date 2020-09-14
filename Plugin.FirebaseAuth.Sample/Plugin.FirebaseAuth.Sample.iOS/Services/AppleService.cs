using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthenticationServices;
using Foundation;
using Plugin.FirebaseAuth.Sample.Services;
using UIKit;

namespace Plugin.FirebaseAuth.Sample.iOS.Services
{
    public class AppleService : IAppleService
    {
        public async Task<(string IdToken, string RawNonce)> GetCredentialAsync()
        {
            if (!Version.TryParse(UIDevice.CurrentDevice.SystemVersion, out var version) || version.Major < 13)
            {
                return (null, null);
            }

            var provider = new ASAuthorizationAppleIdProvider();

            var nonce = GenerateNonce(32);

            var request = provider.CreateRequest();

            request.RequestedScopes = new[] { ASAuthorizationScope.FullName, ASAuthorizationScope.Email };
            request.Nonce = GetHashedNonce(nonce);

            var controller = new ASAuthorizationController(new[] { request });

            var authorizationControllerDelegate = new AuthorizationControllerDelegate(UIApplication.SharedApplication.KeyWindow);

            controller.Delegate = authorizationControllerDelegate;
            controller.PresentationContextProvider = authorizationControllerDelegate;

            controller.PerformRequests();

            var credential = await authorizationControllerDelegate.GetCredentialAsync().ConfigureAwait(false);

            var idToken = new NSString(credential.IdentityToken, NSStringEncoding.UTF8).ToString();

            return (idToken, nonce);
        }

        private string GenerateNonce(int length)
        {
            var charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._".ToCharArray();

            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                var sb = new StringBuilder();

                while (length > 0)
                {
                    rng.GetBytes(bytes);

                    foreach (var b in bytes)
                    {
                        if (length == 0) break;

                        if (b < charset.Length)
                        {
                            sb.Append(charset[b]);
                            length--;
                        }
                    }
                }

                return sb.ToString();
            }
        }

        private string GetHashedNonce(string nonce)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var hachedNonce = sha256.ComputeHash(Encoding.UTF8.GetBytes(nonce));

            var sb = new StringBuilder();
            foreach (var b in hachedNonce)
            {
                sb.Append($"{b:x2}");
            }

            return sb.ToString();
        }

        private class AuthorizationControllerDelegate : ASAuthorizationControllerDelegate, IASAuthorizationControllerPresentationContextProviding
        {
            private readonly TaskCompletionSource<ASAuthorizationAppleIdCredential> _tcs = new TaskCompletionSource<ASAuthorizationAppleIdCredential>();
            private readonly UIWindow _presentationAnchor;

            public AuthorizationControllerDelegate(UIWindow presentationAnchor)
            {
                _presentationAnchor = presentationAnchor;
            }

            public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
            {
                return _presentationAnchor;
            }

            public override void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
            {
                _tcs.TrySetResult(authorization.GetCredential<ASAuthorizationAppleIdCredential>());
            }

            public override void DidComplete(ASAuthorizationController controller, NSError error)
            {
                _tcs.TrySetException(new NSErrorException(error));
            }

            public Task<ASAuthorizationAppleIdCredential> GetCredentialAsync()
            {
                return _tcs.Task;
            }
        }
    }
}
