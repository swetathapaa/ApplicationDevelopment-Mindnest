window.formatText = (editor, command, value) => {
    if (!editor) return;
    editor.focus();
    document.execCommand(command, false, value || null);
};

window.setEditorContent = (editor, html) => {
    if (!editor) return;
    editor.innerHTML = html || "";
};

window.getEditorContent = (editor) => {
    if (!editor) return "";
    return editor.innerHTML;
};
