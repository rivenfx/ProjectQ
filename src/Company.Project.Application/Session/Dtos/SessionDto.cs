using Company.Project.Localization.Dtos;
using Riven.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Session.Dtos
{
    public class SessionDto
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public ClaimsDto Auth { get; set; }

        public LocalizationDto Localization { get; set; }

    }

    public class ClaimsDto
    {
        public List<string> AllClaims { get; set; }


        public List<string> GrantedClaims { get; set; }
    }

    public class LocalizationDto
    {
        public LanguageInfoDto Current { get; set; }

        public List<LanguageInfoDto> Languages { get; set; }

    }
}
