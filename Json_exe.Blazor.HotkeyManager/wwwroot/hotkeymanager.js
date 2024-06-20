let hotkeyManager;

export function initialized(hotkeyManagerInstance, hotkeyManagerOptions) {
    hotkeyManager = hotkeyManagerInstance;
    if (hotkeyManagerOptions.container === null) {
        document.addEventListener('keydown', keyDownEvent);
    } else {
        hotkeyManagerOptions.container.addEventListener('keydown', keyDownEvent);
    }
}

async function keyDownEvent(e) {

}