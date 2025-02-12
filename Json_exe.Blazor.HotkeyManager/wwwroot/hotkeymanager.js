var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
let hotkeyManager;
let options;
export function initialized(hotkeyManagerInstance, hotkeyManagerOptions) {
    hotkeyManager = hotkeyManagerInstance;
    options = new HotkeyManagerOptions(hotkeyManagerOptions.container, hotkeyManagerOptions.hotkeys);
    if (options.container === null) {
        document.addEventListener('keydown', keyDownEvent);
    }
    else {
        options.container.addEventListener('keydown', keyDownEvent);
    }
}
export function dispose() {
    hotkeyManager = null;
    if (options.container === null) {
        document.removeEventListener('keydown', keyDownEvent);
    }
    else {
        options.container.removeEventListener('keydown', keyDownEvent);
    }
    options = null;
}
function keyDownEvent(e) {
    return __awaiter(this, void 0, void 0, function* () {
        if (options.hotkeys.length <= 0) {
            return;
        }
        let hotkey = options.hotkeys.find(h => h.key.toLowerCase() === e.key.toLowerCase() && h.ctrlKey === e.ctrlKey && h.shiftKey === e.shiftKey);
        if (hotkey !== undefined) {
            if (hotkey.preventDefault) {
                e.preventDefault();
            }
            const newObj = {
                ctrlKey: e.ctrlKey,
                shiftKey: e.shiftKey,
                key: e.key,
                code: e.code,
                altKey: e.altKey,
                metaKey: e.metaKey,
                location: e.location,
                type: e.type
            };
            yield hotkeyManager.invokeMethodAsync('OnHotkey', newObj);
        }
    });
}
class HotkeyManagerOptions {
    constructor(container = null, hotkeys = []) {
        this.container = container;
        this.hotkeys = hotkeys;
    }
}
class Hotkey {
    constructor(key, ctrlKey = false, shiftKey = false, preventDefault = false) {
        this.key = key;
        this.ctrlKey = ctrlKey;
        this.shiftKey = shiftKey;
        this.preventDefault = preventDefault;
    }
}
//# sourceMappingURL=hotkeymanager.js.map