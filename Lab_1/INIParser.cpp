#include "INIParser.h"

ParsedINIFile INIParser::Parse(const char **filePath)
{
    INIFileBuilder builder;

    std::ifstream input(filePath[1], std::ios_base::binary);

    if (!input.is_open())
        throw InputFileError();

    std::string section; //data
    std::map<std::string, std::string> keys;
    std::string par;

    std::regex sectionRegExp("\\s*\\[[a-zA-Z0-9_]+\\]\\s*");
    std::regex parKeyRegExp("\\s*[a-zA-Z0-9_]+\\s*=\\s*[a-zA-Z0-9/\\.]+\\s*(;.*)?");
    std::regex commentRegExp("\\s*;.*");
    std::regex whiteSpaceString("\\s*");

    std::string buffer;

    while (!input.eof())
    {
        std::getline(input, buffer);
        if (std::regex_match(buffer, sectionRegExp))
        {
            if (section != "")
            {
                builder.AddSection(section, keys);
                keys.clear();
            }
            auto begin = std::find(buffer.begin(), buffer.end(), '[');
            auto end = std::find(buffer.begin(), buffer.end(), ']');
            section = buffer.substr(std::distance(buffer.begin(), begin + 1),
                                    std::distance(begin, end - 1));
        }
        else if (std::regex_match(buffer, parKeyRegExp))
        {
            if (section == "")
                throw ParseError(buffer.c_str());

            for (auto it = buffer.begin(); it != buffer.end(); it++)
                if (isspace(*it))
                    it = buffer.erase(it) - 1;
                else if (*it == ';')
                    break;

            auto equals = std::find(buffer.begin(), buffer.end(), '=');
            par = buffer.substr(0UL, std::distance(buffer.begin(), equals));

            if (keys.find(par) != keys.end())
                throw ParseError(par.c_str());

            auto comm = std::find(buffer.begin(), buffer.end(), ';');
            keys[par] = buffer.substr(std::distance(buffer.begin(), equals + 1),
                                      std::distance(equals, comm - 1));
        }
        else if (std::regex_match(buffer, commentRegExp) || std::regex_match(buffer, whiteSpaceString))
        {
            continue;
        }
        else
        {
            throw ParseError(buffer.c_str());
        }
    }

    if (!keys.empty())
    {
        builder.AddSection(section, keys);
        keys.clear();
    }

    return ParsedINIFile(builder.getData());
}
