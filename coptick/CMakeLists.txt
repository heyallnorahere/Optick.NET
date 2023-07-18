cmake_minimum_required(VERSION 3.10 FATAL_ERROR)

add_library(coptick SHARED coptick.cpp)
target_link_libraries(coptick PRIVATE OptickCore)

set_target_properties(coptick PROPERTIES
    CXX_STANDARD 17)