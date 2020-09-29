#pragma once
#include <fstream>
#include <string>
#include <regex>
#include <iostream>
#include <cctype>
#include <string.h>
#include "ParsedINIFile.h"
#include "INIFileBuilder.h"
#include "MyException.h"

class INIParser
{
public:
    ParsedINIFile Parse(const char **filePath);
};

