let hotkeyManager;
let options;

export function initialized(hotkeyManagerInstance, hotkeyManagerOptions) {
    hotkeyManager = hotkeyManagerInstance;
    options = new HotkeyManagerOptions(hotkeyManagerOptions.container, hotkeyManagerOptions.hotkeys);
    if (options.container === null) {
        document.addEventListener('keydown', keyDownEvent);
    } else {
        options.container.addEventListener('keydown', keyDownEvent);
    }
}

async function keyDownEvent(e) {
    if (options.hotkeys.length <= 0) {
        return
    }
    let hotkey = options.hotkeys.find(h => h.key.toLowerCase() === e.key && h.ctrlKey === e.ctrlKey && h.shiftKey === e.shiftKey);
    console.log(hotkey)
    if (hotkey !== undefined) {
        if (hotkey.preventDefault) {
            e.preventDefault()
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