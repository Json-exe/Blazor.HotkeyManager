class DisposeElement extends HTMLElement {
    static readonly TagName = "dispose-element";
    eventTarget = new EventTarget()
    disposed = false

    connectedCallback() {
        this.style.display = "none";
        this.style.width = "0";
        this.style.height = "0";
    }

    disconnectedCallback() {
        this.dispose()
    }

    addEventListener(type: string, listener: EventListenerOrEventListenerObject | null, options?: EventListenerOptions | boolean) {
        if (type !== "dispose") throw new DOMException("Element only accepts a dispose event listener.");
        this.eventTarget.addEventListener(type, listener, options)
    }

    removeEventListener(type: string, listener: EventListenerOrEventListenerObject | null, options?: EventListenerOptions | boolean) {
        this.eventTarget.removeEventListener(type, listener, options)
    }

    removeAllDisposeListener() {
        if (this.disposed) throw new DOMException("Element is already disposed.");
        this.eventTarget.dispatchEvent(new Event("dispose"))
        this.eventTarget = new EventTarget();
    }

    onDisposed(cb: EventListenerOrEventListenerObject) {
        if (this.disposed) throw new DOMException("Element is already disposed.")
        this.addEventListener("dispose", cb)
        return () => this.removeEventListener("dispose", cb)
    }

    dispose() {
        if (this.disposed) return
        this.disposed = true
        this.eventTarget.dispatchEvent(new Event("dispose"))
    }
}

export class HotkeyManager {
    private hotkeyManager;
    private options: HotkeyManagerOptions;
    private disposeElement?: DisposeElement;
    private readonly keyDownFunction = this.keyDownEvent.bind(this);
    private disposeElementCleanupFunction: () => void;

    public constructor(hotkeyManagerInstance, hotkeyManagerOptions: HotkeyManagerOptions) {
        this.hotkeyManager = hotkeyManagerInstance;
        this.initialize(hotkeyManagerOptions);
    }

    public initialize(hotkeyManagerOptions: HotkeyManagerOptions) {
        this.tryDefineDisposeElement();
        this.options = new HotkeyManagerOptions(hotkeyManagerOptions.container, hotkeyManagerOptions.hotkeys);
        if (!this.disposeElement) {
            this.disposeElement = document.createElement(DisposeElement.TagName) as DisposeElement;
            this.disposeElementCleanupFunction = this.disposeElement.onDisposed(this.dispose.bind(this));
        }
        if (this.options.container === null) {
            document.addEventListener('keydown', this.keyDownFunction);
            document.body.appendChild(this.disposeElement);
        } else {
            this.options.container.addEventListener('keydown', this.keyDownFunction);
            this.options.container.appendChild(this.disposeElement);
        }
    }

    private tryDefineDisposeElement() {
        const customElement = customElements.get(DisposeElement.TagName)
        if (customElement !== undefined) return;
        customElements.define(DisposeElement.TagName, DisposeElement)
    }

    private dispose() {
        this.hotkeyManager = null;
        if (this.options.container) {
            this.options.container.removeEventListener('keydown', this.keyDownFunction);
        } else {
            document.removeEventListener('keydown', this.keyDownFunction);
        }

        this.options = null;
        this.disposeElementCleanupFunction();
        this.disposeElement = null;
    }

    private async keyDownEvent(e: KeyboardEvent) {
        if (this.options.hotkeys.length <= 0) {
            return
        }
        let hotkey = this.options.hotkeys.find(h => h.key.toLowerCase() === e.key.toLowerCase() && h.ctrlKey === e.ctrlKey && h.shiftKey === e.shiftKey);
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
            await this.hotkeyManager.invokeMethodAsync('OnHotkey', newObj, hotkey.id);
        }
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
    public id: string /* Guid */;
    public key: string;
    public ctrlKey: boolean;
    public shiftKey: boolean;
    public altKey: boolean;
    public preventDefault: boolean;

    constructor(id: string /* Guid */, key: string, ctrlKey = false, shiftKey = false, altKey = false, preventDefault = false) {
        this.id = id;
        this.key = key;
        this.ctrlKey = ctrlKey;
        this.shiftKey = shiftKey;
        this.altKey = altKey;
        this.preventDefault = preventDefault;
    }
}