let hotkeyManager;
let options = new HotkeyManagerOptions();

export function initialized(hotkeyManagerInstance, hotkeyManagerOptions) {
    hotkeyManager = hotkeyManagerInstance;
    if (hotkeyManagerOptions.container === null) {
        document.addEventListener('keydown', keyDownEvent);
    } else {
        hotkeyManagerOptions.container.addEventListener('keydown', keyDownEvent);
    }

    options = new HotkeyManagerOptions(hotkeyManagerOptions.container, hotkeyManagerOptions.hotkeys);
}

async function keyDownEvent(e) {
    let hotkey = options.hotkeys.find(h => h.key === e.key && h.ctrlKey === e.ctrlKey && h.shiftKey === e.shiftKey);
    if (hotkey !== undefined) {
        if (hotkey.preventDefault) { 
            e.preventDefault() 
        };
        const newObj = {
            ctrlKey: e.ctrlKey,
            shiftKey: e.shiftKey,
            key: e.key,
            code: e.code,
            altKey: e.altKey,
            metaKey: e.metaKey,
            location: e.location,
            type: e.type
        }
        await hotkeyManager.invokeMethodAsync('OnHotkey', newObj);
    }
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