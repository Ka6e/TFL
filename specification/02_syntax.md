02 — Синтаксис языка Swiston

1. Общая структура программы
	•	Программа состоит из:
	1	произвольного числа объявлений функций
	2	ровно одной функции main

⸻

2. Приоритет и ассоциативность операторов

От высшего к низшему:

Приоритет	Операторы	Ассоциативность
1	!	правая
2	* / %	левая
3	+ -	левая
4	< <= > >=	левая
5	== !=	левая
6	&&	левая
7	`	


⸻

3. Полная ISO EBNF-грамматика синтаксиса

program         = { function_decl }, main_decl ;

main_decl       =
    "main",
    block ;

function_decl   =
    "func",
    identifier,
    "(",
    [ parameter_list ],
    ")",
    ":",
    type,
    block ;

parameter_list  =
    parameter,
    { ",", parameter } ;

parameter       =
    identifier, ":", type ;

type            =
      "int"
    | "float"
    | "string"
    | "bool" ;

block           =
    "{",
    { statement },
    "}" ;

statement       =
      var_decl
    | const_decl
    | assignment
    | if_stmt
    | while_stmt
    | return_stmt
    | break_stmt
    | continue_stmt
    | expr_stmt ;

var_decl        =
    "var",
    identifier,
    ":",
    type,
    [ "=", expression ],
    ";" ;

const_decl      =
    "const",
    identifier,
    ":",
    type,
    "=",
    expression,
    ";" ;

assignment      =
    identifier,
    "=",
    expression,
    ";" ;

if_stmt         =
    "if",
    "(",
    expression,
    ")",
    block,
    [ "else", block ] ;

while_stmt      =
    "while",
    "(",
    expression,
    ")",
    block ;

return_stmt     =
    "return",
    [ expression ],
    ";" ;

break_stmt      =
    "break",
    ";" ;

continue_stmt   =
    "continue",
    ";" ;

expr_stmt       =
    expression,
    ";" ;

expression      = logical_or ;

logical_or      =
    logical_and,
    { "||", logical_and } ;

logical_and     =
    equality,
    { "&&", equality } ;

equality        =
    comparison,
    { ( "==" | "!=" ), comparison } ;

comparison      =
    term,
    { ( "<" | "<=" | ">" | ">=" ), term } ;

term            =
    factor,
    { ( "+" | "-" ), factor } ;

factor          =
    unary,
    { ( "*" | "/" | "%" ), unary } ;

unary           =
      "!", unary
    | primary ;

primary         =
      literal
    | identifier
    | function_call
    | "(", expression, ")" ;

function_call   =
    identifier,
    "(",
    [ argument_list ],
    ")" ;

argument_list   =
    expression,
    { ",", expression } ;

literal         =
      int_literal
    | float_literal
    | string_literal
    | bool_literal ;

