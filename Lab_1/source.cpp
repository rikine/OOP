#include <iostream>
#include <string>
#include <algorithm>
#include "INIParser.h"

int main(int argc, const char **argv)
{
    if (argc != 2)
    {
        std::cerr << "Usage: ./INIParser /home/rikine/Documents/FILE.ini" << '\n';
        return 1;
    }

    ParsedINIFile iniFile;

    try
    {
        iniFile = INIParser().Parse(argv);
    }
    catch (const std::exception &e)
    {
        std::cerr << e.what() << '\n';
        return 2;
    }

    std::string type, section, key; //input

    std::cout << "Input name of section: ";
    std::cin >> section;

    std::cout << "Input name of key: ";
    std::cin >> key;

    std::cout << "Input type of the value: ";
    std::cin >> type;

    int resInt = 0;
    float resFloat = 0.0f;
    std::string resString = "";

    try
    {
        std::transform(type.begin(), type.end(), type.begin(), tolower);
        if (type == "int")
        {
            resInt = iniFile.GetInt(section, key);
            std::cout
                << '[' << section << "]\n"
                << key << " = " << resInt << '\n';
        }
        else if (type == "float")
        {
            resFloat = iniFile.GetFloat(section, key);
            std::cout
                << '[' << section << "]\n"
                << key << " = " << resFloat << '\n';
        }
        else if (type == "string")
        {
            resString = iniFile.GetString(section, key);
            std::cout
                << '[' << section << "]\n"
                << key << " = " << resString << '\n';
        }
        else
        {
            std::cerr << "Wrong type of value. (int, float, string)" << '\n';
            return 3;
        }
    }
    catch (const GettingParamError &e)
    {
        std::cerr << '\n'
                  << e.what() << '\n';
        return 4;
    }
    catch (const std::exception &)
    {
        std::cerr << '\n'
                  << "Something wrong with a value convering to " << type << " : "
                  << iniFile.GetString(section, key) << '\n';
        return 4;
    }
    return 0;
}