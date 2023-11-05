mergeInto(LibraryManager.library, {
    _DeleteAll: function () {
        localStorage.clear();
    },
    _Clean: function () {
        const date = new Date().getTime();
        const keys = [];
        for (const key in localStorage) {
            keys.push(key);
        }
        for (const key in keys) {
            const expirationKey = key + ".expire";
            const expiration = localStorage.getItem(expirationKey);
            if (expiration === null) {
                continue;
            }

            if (date < parseInt(expiration)) {
                continue;
            }
            localStorage.removeItem(key);
            localStorage.removeItem(expirationKey);
        }
    },
    _Write: function (file, data) {
        localStorage.setItem(UTF8ToString(file), UTF8ToString(data));
    },
    _WriteTemporary: function (file, data, expiration) {
        const key = UTF8ToString(file);
        const value = UTF8ToString(data);
        const expirationTimestamp = Date.parse(UTF8ToString(expiration));
        localStorage.setItem(key, value);
        localStorage.setItem(key + ".expire", expiration);
    },
    _Read: function (file) {
        const data = localStorage.getItem(UTF8ToString(file)) || "";
        const bufferSize = lengthBytesUTF8(data) + 1;
        const buffer = _malloc(bufferSize);
        stringToUTF8(data, buffer, bufferSize);
        return buffer;
    }
});