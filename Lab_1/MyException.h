#pragma once
#include <exception>
#include <string.h>

class MyException : public std::exception
{
    const char *msg_;

public:
    MyException(const char *msg1, const char *msg2 = "", const char *msg3 = "", const char *msg4 = "");

    const char *what() const noexcept override { return msg_; }

    ~MyException() override { delete[] msg_; }
};

class GettingParamError : public MyException
{
public:
    GettingParamError(const char *msg1, const char *msg2) : MyException(msg1, msg2) {}
};