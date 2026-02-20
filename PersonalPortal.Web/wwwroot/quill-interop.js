window.quillInterop = {
    quillInstances: {},
    
    createQuill: function(elementId, readOnly, dotNetHelper) {
        var container = document.getElementById(elementId);
        if (!container) {
            console.error('Quill container not found:', elementId);
            return false;
        }

        var quill = new Quill('#' + elementId, {
            theme: 'snow',
            readOnly: readOnly,
            modules: {
                toolbar: readOnly ? false : [
                    [{ 'header': [1, 2, 3, false] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                    [{ 'indent': '-1'}, { 'indent': '+1' }],
                    ['link'],
                    ['clean']
                ]
            }
        });

        this.quillInstances[elementId] = quill;

        if (dotNetHelper) {
            quill.on('text-change', function() {
                var html = quill.root.innerHTML;
                dotNetHelper.invokeMethodAsync('OnContentChanged', html);
            });
        }

        return true;
    },

    setQuillContent: function(elementId, html) {
        var quill = this.quillInstances[elementId];
        if (quill) {
            quill.root.innerHTML = html || '';
        }
    },

    getQuillContent: function(elementId) {
        var quill = this.quillInstances[elementId];
        if (quill) {
            return quill.root.innerHTML;
        }
        return '';
    },

    destroyQuill: function(elementId) {
        delete this.quillInstances[elementId];
    },

    clearFileInput: function(elementId) {
        var fileInput = document.getElementById(elementId);
        if (fileInput) {
            fileInput.value = '';
        }
    }
};
