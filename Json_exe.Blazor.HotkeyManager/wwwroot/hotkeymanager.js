var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class DisposeElement extends HTMLElement {
    constructor() {
        super(...arguments);
        this.eventTarget = new EventTarget();
        this.disposed = false;
        this.isMoving = false;
    }
    connectedCallback() {
        this.isMoving = false;
        this.style.display = "none";
        this.style.width = "0";
        this.style.height = "0";
    }
    disconnectedCallback() {
        if (this.isMoving)
            return;
        this.dispose();
    }
    connectedMoveCallback() {
    }
    addEventListener(type, listener, options) {
        if (type !== "dispose")
            throw new DOMException("Element only accepts a dispose event listener.");
        this.eventTarget.addEventListener(type, listener, options);
    }
    removeEventListener(type, listener, options) {
        this.eventTarget.removeEventListener(type, listener, options);
    }
    removeAllDisposeListener() {
        if (this.disposed)
            throw new DOMException("Element is already disposed.");
        this.eventTarget.dispatchEvent(new Event("dispose"));
        this.eventTarget = new EventTarget();
    }
    onDisposed(cb) {
        if (this.disposed)
            throw new DOMException("Element is already disposed.");
        this.addEventListener("dispose", cb);
        return () => this.removeEventListener("dispose", cb);
    }
    dispose() {
        if (this.disposed)
            return;
        this.disposed = true;
        this.eventTarget.dispatchEvent(new Event("dispose"));
    }
}
DisposeElement.TagName = "dispose-element";
export class HotkeyManager {
    constructor(hotkeyManagerInstance, hotkeyManagerOptions) {
        this.keyDownFunction = this.keyDownEvent.bind(this);
        this.hotkeyManager = hotkeyManagerInstance;
        this.initialize(hotkeyManagerOptions);
    }
    initialize(hotkeyManagerOptions) {
        this.tryDefineDisposeElement();
        this.dispose(false);
        this.options = new HotkeyManagerOptions(hotkeyManagerOptions.container, hotkeyManagerOptions.hotkeys);
        if (!this.disposeElement) {
            this.disposeElement = document.createElement(DisposeElement.TagName);
            this.disposeElementCleanupFunction = this.disposeElement.onDisposed(() => this.dispose(true));
        }
        if (this.disposeElement.isConnected)
            this.disposeElement.isMoving = true;
        if (this.options.container === null) {
            document.addEventListener('keydown', this.keyDownFunction);
            document.body.appendChild(this.disposeElement);
        }
        else {
            this.options.container.addEventListener('keydown', this.keyDownFunction);
            this.options.container.appendChild(this.disposeElement);
        }
    }
    tryDefineDisposeElement() {
        const customElement = customElements.get(DisposeElement.TagName);
        if (customElement !== undefined)
            return;
        customElements.define(DisposeElement.TagName, DisposeElement);
    }
    dispose(disposing) {
        var _a;
        if ((_a = this.options) === null || _a === void 0 ? void 0 : _a.container) {
            this.options.container.removeEventListener('keydown', this.keyDownFunction);
        }
        else {
            document.removeEventListener('keydown', this.keyDownFunction);
        }
        if (disposing) {
            this.disposeElementCleanupFunction();
            this.hotkeyManager = null;
            this.options = null;
            this.disposeElement = null;
        }
    }
    keyDownEvent(e) {
        return __awaiter(this, void 0, void 0, function* () {
            if (this.options.hotkeys.length <= 0) {
                return;
            }
            let hotkey = this.options.hotkeys.find(h => h.key.toLowerCase() === e.key.toLowerCase()
                && h.ctrlKey === e.ctrlKey
                && h.shiftKey === e.shiftKey
                && h.altKey === e.altKey);
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
                yield this.hotkeyManager.invokeMethodAsync('OnHotkey', newObj, hotkey.id);
            }
        });
    }
}
class HotkeyManagerOptions {
    constructor(container = null, hotkeys = []) {
        this.container = container;
        this.hotkeys = hotkeys;
    }
}
class Hotkey {
    constructor(id /* Guid */, key, ctrlKey = false, shiftKey = false, altKey = false, preventDefault = false) {
        this.id = id;
        this.key = key;
        this.ctrlKey = ctrlKey;
        this.shiftKey = shiftKey;
        this.altKey = altKey;
        this.preventDefault = preventDefault;
    }
}
//# sourceMappingURL=hotkeymanager.js.map