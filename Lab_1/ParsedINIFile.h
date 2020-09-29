#pragma once
#include <map>
#include <string>
#include "MyException.h"

class ParsedINIFile
{
    std::map<std::string, std::map<std::string, std::string>> data;

public:
    ParsedINIFile() {}
    ParsedINIFile(const std::map<std::string, std::map<std::string, std::string>> &data) : data(data) {}

    int GetInt(const std::string &section, const std::string &key);
    float GetFloat(const std::string &section, const std::string &key);
    std::string GetString(const std::string &section, const std::string &key);

private:
    void CheckingExisence(const std::string &section, const std::string &key);
};