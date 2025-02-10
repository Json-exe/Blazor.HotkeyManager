let hotkeyManager;
let options: HotkeyManagerOptions;

export function initialized(hotkeyManagerInstance, hotkeyManagerOptions: HotkeyManagerOptions) {
    hotkeyManager = hotkeyManagerInstance;
    options = new HotkeyManagerOptions(hotkeyManagerOptions.container, hotkeyManagerOptions.hotkeys);
    if (options.container === null) {
        document.addEventListener('keydown', keyDownEvent);
    } else {
        options.container.addEventListener('keydown', keyDownEvent);
    }
}

export function dispose() {
    hotkeyManager = null;
    if (options.container === null) {
        document.removeEventListener('keydown', keyDownEvent);
    } else {
        options.container.removeEventListener('keydown', keyDownEvent);
    }

    options = null;
}

async function keyDownEvent(e: KeyboardEvent) {
    if (options.hotkeys.length <= 0) {
        return
    }
    let hotkey = options.hotkeys.find(h => h.key.toLowerCase() === e.key && h.ctrlKey === e.ctrlKey && h.shiftKey === e.shiftKey);
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
    public container?: HTMLElement;
    public hotkeys: Hotkey[];

    constructor(container: HTMLElement = null, hotkeys: Hotkey[] = []) {
        this.container = container;
        this.hotkeys = hotkeys;
    }
}

class Hotkey {
    public key: string;
    public ctrlKey: boolean;
    public shiftKey: boolean;
    public preventDefault: boolean;

    constructor(key: string, ctrlKey = false, shiftKey = false, preventDefault = false) {
        this.key = key;
        this.ctrlKey = ctrlKey;
        this.shiftKey = shiftKey;
        this.preventDefault = preventDefault;
    }
}