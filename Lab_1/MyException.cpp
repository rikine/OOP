#include "MyException.h"

ParseError::ParseError(const char *msg)
{
    msg_ = new char[150];
    strcpy(msg_, msg);
}
