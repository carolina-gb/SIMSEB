using SIMSEB.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Utils
{
    public class UsernameHelper
    {
        private readonly IUserRepository _userRepository;

        public UsernameHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> GenerateUniqueUsernameAsync(string fullName, string fullLastName)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(fullLastName))
                throw new ArgumentException("El nombre o apellido no puede estar vacío.");

            // Primera letra del primer nombre
            var firstLetter = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)[0][0];

            // Primer apellido
            var firstLastName = fullLastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];

            // Construir base del username
            var baseUsername = RemoveDiacritics($"{firstLetter}{firstLastName}").ToLowerInvariant();
            var username = baseUsername;

            int counter = 1;

            // Validar existencia usando tu función
            while (await _userRepository.GetByEmailOrUsernameAsync(username) != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            return username;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var ch in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
