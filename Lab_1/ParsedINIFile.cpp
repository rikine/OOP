#include "ParsedINIFile.h"

int ParsedINIFile::GetInt(const std::string &section, const std::string &key)
{
    CheckingExisence(section, key);
    return std::stoi(data[section][key]);
}

float ParsedINIFile::GetFloat(const std::string &section, const std::string &key)
{
    CheckingExisence(section, key);
    return std::stof(data[section][key]);
}

std::string ParsedINIFile::GetString(const std::string &section, const std::string &key)
{
    CheckingExisence(section, key);
    return data[section][key];
}

void ParsedINIFile::CheckingExisence(const std::string &section, const std::string &key)
{
    if (data.find(section) == data.end())
        throw GetParamError();
    if (data[section].find(key) == data[section].end())
        throw GetParamError();
}
