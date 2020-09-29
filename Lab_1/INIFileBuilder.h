#pragma once
#include <map>
#include <string>
#include <string.h>
#include "MyException.h"

class INIFileBuilder
{
    std::map<std::string, std::map<std::string, std::string>> data;

public:
    INIFileBuilder() {}

    void AddSection(const std::string &sectionName, const std::map<std::string, std::string> &keys);

    std::map<std::string, std::map<std::string, std::string>> &getData() { return data; }
};