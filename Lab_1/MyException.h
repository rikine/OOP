#pragma once
#include <exception>
#include <string.h>

class GetParamError : public std::exception
{
public:
    GetParamError() = default;

    const char *what() const noexcept override { return "No such section in this file "
                                                        "or no such key in this section."; }
};

class InputFileError : public std::exception
{
public:
    InputFileError() = default;

    const char *what() const noexcept override { return "File isn't open. Example: ./INIParser "
                                                        "/home/rikine/Documents/FILE.ini"; }
};

class ParseError : public std::exception
{
    char *msg_;

public:
    ParseError(const char *msg);

    const char *what() const noexcept override { return "Invalid file. Parsing error with line: "; }

    char *moreInfo() const noexcept { return msg_; }

    ~ParseError() { delete[] msg_; }
};

/*class MyException : public std::exception
{
    const char *msg_;

public:
    MyException(const char *msg) : msg_(msg) {}

    const char *what() const noexcept override { return msg_; }

    ~MyException() override { delete[] msg_; }
};*/
