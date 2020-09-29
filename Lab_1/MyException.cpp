#include "MyException.h"

MyException::MyException(const char *msg1, const char *msg2, const char *msg3, const char *msg4)
{
    char *msg = new char[200];
    msg = strcat(msg, msg1);
    msg = strcat(msg, msg2);
    msg = strcat(msg, msg3);
    msg = strcat(msg, msg4);
    msg_ = msg;
}
