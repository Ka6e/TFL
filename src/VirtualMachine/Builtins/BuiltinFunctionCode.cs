namespace VirtualMachine.Builtins;

/// <summary>
/// Код встроенной функции.
/// </summary>
public enum BuiltinFunctionCode
{
    /// <summary>
    /// `print(i: int)` — выводит целое число в стандартный поток вывода
    /// </summary>
    Print = 1,

    /// <summary>
    /// readi() -> int — читает целое число из стандартного потока ввода и возвращает его
    /// </summary>
    ReadI = 2,
}