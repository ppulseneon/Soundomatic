using System.Collections.Generic;
using SharpHook.Data;

namespace Soundomatic.Helpers;

/// <summary>
/// Хелпер для получения user-friendly названий для кодов клавиш
/// </summary>
public class KeyCodeHelper
{
    /// <summary>
    /// Словарь для преобразования KeyCode в string значение
    /// </summary>
    private static readonly Dictionary<KeyCode, string> FriendlyNamesMap = new()
    {
        [KeyCode.VcUndefined] = "Не задано",
        [KeyCode.VcEscape] = "Escape",
        [KeyCode.VcTab] = "Tab",
        [KeyCode.VcCapsLock] = "Caps Lock",
        [KeyCode.VcLeftShift] = "Левый Shift",
        [KeyCode.VcRightShift] = "Правый Shift",
        [KeyCode.VcLeftControl] = "Левый Control",
        [KeyCode.VcRightControl] = "Правый Control",
        [KeyCode.VcLeftAlt] = "Левый Alt",
        [KeyCode.VcRightAlt] = "Правый Alt",
        [KeyCode.VcLeftMeta] = "Левая клавиша Win/Cmd",
        [KeyCode.VcRightMeta] = "Правая клавиша Win/Cmd",
        [KeyCode.VcContextMenu] = "Контекстное меню",
        [KeyCode.VcEnter] = "Enter",
        [KeyCode.VcSpace] = "Пробел",
        [KeyCode.VcBackspace] = "Backspace",
        [KeyCode.VcInsert] = "Insert",
        [KeyCode.VcDelete] = "Delete",

        [KeyCode.VcUp] = "Стрелка вверх",
        [KeyCode.VcDown] = "Стрелка вниз",
        [KeyCode.VcLeft] = "Стрелка влево",
        [KeyCode.VcRight] = "Стрелка вправо",
        [KeyCode.VcHome] = "Home",
        [KeyCode.VcEnd] = "End",
        [KeyCode.VcPageUp] = "Page Up",
        [KeyCode.VcPageDown] = "Page Down",

        [KeyCode.VcNumLock] = "Num Lock",
        [KeyCode.VcNumPad0] = "0 (Numpad)",
        [KeyCode.VcNumPad1] = "1 (Numpad)",
        [KeyCode.VcNumPad2] = "2 (Numpad)",
        [KeyCode.VcNumPad3] = "3 (Numpad)",
        [KeyCode.VcNumPad4] = "4 (Numpad)",
        [KeyCode.VcNumPad5] = "5 (Numpad)",
        [KeyCode.VcNumPad6] = "6 (Numpad)",
        [KeyCode.VcNumPad7] = "7 (Numpad)",
        [KeyCode.VcNumPad8] = "8 (Numpad)",
        [KeyCode.VcNumPad9] = "9 (Numpad)",
        [KeyCode.VcNumPadEnter] = "Enter (Numpad)",
        [KeyCode.VcNumPadAdd] = "+ (Numpad)",
        [KeyCode.VcNumPadSubtract] = "- (Numpad)",
        [KeyCode.VcNumPadMultiply] = "* (Numpad)",
        [KeyCode.VcNumPadDivide] = "/ (Numpad)",
        [KeyCode.VcNumPadDecimal] = ". (Numpad)",
        [KeyCode.VcNumPadSeparator] = "Разделитель (Numpad)",
        [KeyCode.VcNumPadEquals] = "= (Numpad)",
        [KeyCode.VcNumPadClear] = "Очистить (Numpad)",

        [KeyCode.VcF1] = "F1",
        [KeyCode.VcF2] = "F2",
        [KeyCode.VcF3] = "F3",
        [KeyCode.VcF4] = "F4",
        [KeyCode.VcF5] = "F5",
        [KeyCode.VcF6] = "F6",
        [KeyCode.VcF7] = "F7",
        [KeyCode.VcF8] = "F8",
        [KeyCode.VcF9] = "F9",
        [KeyCode.VcF10] = "F10",
        [KeyCode.VcF11] = "F11",
        [KeyCode.VcF12] = "F12",
        [KeyCode.VcF13] = "F13",
        [KeyCode.VcF14] = "F14",
        [KeyCode.VcF15] = "F15",
        [KeyCode.VcF16] = "F16",
        [KeyCode.VcF17] = "F17",
        [KeyCode.VcF18] = "F18",
        [KeyCode.VcF19] = "F19",
        [KeyCode.VcF20] = "F20",
        [KeyCode.VcF21] = "F21",
        [KeyCode.VcF22] = "F22",
        [KeyCode.VcF23] = "F23",
        [KeyCode.VcF24] = "F24",

        [KeyCode.Vc0] = "0", [KeyCode.Vc1] = "1", [KeyCode.Vc2] = "2",
        [KeyCode.Vc3] = "3", [KeyCode.Vc4] = "4", [KeyCode.Vc5] = "5",
        [KeyCode.Vc6] = "6", [KeyCode.Vc7] = "7", [KeyCode.Vc8] = "8", [KeyCode.Vc9] = "9",
        [KeyCode.VcA] = "A", [KeyCode.VcB] = "B", [KeyCode.VcC] = "C",
        [KeyCode.VcD] = "D", [KeyCode.VcE] = "E", [KeyCode.VcF] = "F",
        [KeyCode.VcG] = "G", [KeyCode.VcH] = "H", [KeyCode.VcI] = "I",
        [KeyCode.VcJ] = "J", [KeyCode.VcK] = "K", [KeyCode.VcL] = "L",
        [KeyCode.VcM] = "M", [KeyCode.VcN] = "N", [KeyCode.VcO] = "O",
        [KeyCode.VcP] = "P", [KeyCode.VcQ] = "Q", [KeyCode.VcR] = "R",
        [KeyCode.VcS] = "S", [KeyCode.VcT] = "T", [KeyCode.VcU] = "U",
        [KeyCode.VcV] = "V", [KeyCode.VcW] = "W", [KeyCode.VcX] = "X",
        [KeyCode.VcY] = "Y", [KeyCode.VcZ] = "Z",
        [KeyCode.VcComma] = ",",
        [KeyCode.VcMinus] = "-",
        [KeyCode.VcPeriod] = ".",
        [KeyCode.VcSlash] = "/",
        [KeyCode.VcSemicolon] = ";",
        [KeyCode.VcEquals] = "=",
        [KeyCode.VcOpenBracket] = "[",
        [KeyCode.VcCloseBracket] = "]",
        [KeyCode.VcBackslash] = "\\",
        [KeyCode.VcBackQuote] = "`",
        [KeyCode.VcQuote] = "'",
        [KeyCode.VcUnderscore] = "_",

        [KeyCode.VcPrintScreen] = "Print Screen",
        [KeyCode.VcScrollLock] = "Scroll Lock",
        [KeyCode.VcPause] = "Pause",
        [KeyCode.VcHelp] = "Help",
        [KeyCode.VcCancel] = "Отмена",
        [KeyCode.VcPower] = "Питание",
        [KeyCode.VcSleep] = "Сон",
        [KeyCode.VcMediaNext] = "Следующий трек",
        [KeyCode.VcMediaPrevious] = "Предыдущий трек",
        [KeyCode.VcMediaStop] = "Стоп",
        [KeyCode.VcMediaPlay] = "Воспр./Пауза",
        [KeyCode.VcMediaEject] = "Извлечь диск",
        [KeyCode.VcVolumeUp] = "Громкость +",
        [KeyCode.VcVolumeDown] = "Громкость -",
        [KeyCode.VcVolumeMute] = "Выключить звук",
        
        [KeyCode.VcAppMail] = "Запустить почту",
        [KeyCode.VcAppCalculator] = "Запустить калькулятор",
        [KeyCode.VcAppBrowser] = "Запустить браузер",
        [KeyCode.VcBrowserHome] = "Домой (браузер)",
        [KeyCode.VcBrowserSearch] = "Поиск (браузер)",
        [KeyCode.VcBrowserFavorites] = "Избранное (браузер)",
        [KeyCode.VcBrowserRefresh] = "Обновить (браузер)",
        [KeyCode.VcBrowserStop] = "Стоп (браузер)",
        [KeyCode.VcBrowserForward] = "Вперед (браузер)",
        [KeyCode.VcBrowserBack] = "Назад (браузер)",
        
        [KeyCode.VcFunction] = "Fn",
        [KeyCode.Vc102] = "Доп. клавиша (ISO <>/\\|)",
        [KeyCode.VcYen] = "¥",
        [KeyCode.VcJpComma] = "Японская запятая",
        [KeyCode.VcKana] = "Режим Kana (IME)",
        [KeyCode.VcKanji] = "Режим Kanji (IME)",
        [KeyCode.VcConvert] = "Конвертировать (IME)",
        [KeyCode.VcNonConvert] = "Без конвертации (IME)",
        [KeyCode.VcAccept] = "Принять (IME)",
        [KeyCode.VcHanja] = "Режим Hanja (IME)",
        [KeyCode.VcHangul] = "Режим Hangul (IME)",
        [KeyCode.VcModeChange] = "Смена режима (IME)",
    };

    /// <summary>
    /// Возвращает user-friendly название для указанного кода клавиши
    /// </summary>
    public static string GetFriendlyName(KeyCode keyCode) => FriendlyNamesMap.TryGetValue(keyCode, out var name) ? name :
        keyCode.ToString();

    /// <summary>
    /// Проверяет, назначен ли код клавиши
    /// </summary>
    /// <returns>Возвращает True, если код клавиши назначен</returns>
    public static bool IsKeyAssigned(KeyCode keyCode) => keyCode != KeyCode.VcUndefined;
}