var seleniumIdCounter = 0;

function GetXpathArray(xpath, isArrayAsText, contextNode) {

    var searchContex;
    if (contextNode == null) {
        searchContex = document;
    }
    else {
        searchContex = contextNode;
    }

    var result = document.evaluate(xpath, searchContex, null, XPathResult.ANY_TYPE, null);
    var rez;
    if (!result) {
        return null;
    }
    switch (result.resultType) {
        case XPathResult.NUMBER_TYPE:
            rez = ReturnSingleNumber(result);
            break;
        case XPathResult.STRING_TYPE:
            rez = ReturnSingleString(result);
            break;
        case XPathResult.UNORDERED_NODE_ITERATOR_TYPE:
            rez = ReturnAsArray(result, isArrayAsText);
            break;
        default:
            rez = 'Not expected result type ' + result.resultTyp;
    }
    return rez;
}




function ReturnSingleString(xpathResult) {
    return xpathResult.stringValue;
}

function ReturnSingleNumber(xpathResult) {
    return xpathResult.numberValue;
}

function ReturnAsArray(xpathResult, asText) {
    var result = [];
    var next = xpathResult.iterateNext();
    while (next) {
        try {
            var value;
            if (asText == true) {
                value = next.textContent;
            }
            else {
                value = next;
            }

            result.push(value);
            next = xpathResult.iterateNext();
        }
        catch (err) {
        }
    }
    return result;
}
var RootElement;

function setRootElement(root) {
    if (root == null) {
        RootElement = document;
    }
    else {

        RootElement = root;
    }
}

function getAllShadowDomDocumentFragments(rooElement) {

    if (rooElement == null) {
        ;
        rooElement = document;
    }

    var shdw = [];
    var childrensArr = [];
    if (rooElement.shadowRoot != null) {

        shdw.push(rooElement.shadowRoot);
        if (rooElement.shadowRoot.children != null) {
            var shadowChildrens = rooElement.shadowRoot.children;
            for (var c = 0; c < shadowChildrens.length; c++) {
                childrensArr = [];
                childrensArr = (getAllShadowDomDocumentFragments(shadowChildrens[c]));
                shdw = shdw.concat(childrensArr);
            }
        }
    }
    if (rooElement.children != null) {
        for (var c = 0; c < rooElement.children.length; c++) {
            childrensArr = (getAllShadowDomDocumentFragments(rooElement.children[c]));
            shdw = shdw.concat(childrensArr);
            childrensArr = [];
        }

    }
    return shdw;
}

function searchForElementByXpath(xpathString, rootElement) {
    /* Search is not possible with by xpath in document-fragment, so in this case search by xpath will be done in temp element with document-fragment innerHtml and if any nodes are found then there will be returned document fragment that contains desired elements */
    var fragmentsArray = getAllShadowDomDocumentFragments(rootElement);
    setCustomAttributesForAllFields(fragmentsArray);
    var xpathResultsArray = [];
    var filteredFragmentsArray = [];
    var cssResults = [];
    for (var a = 0; a < fragmentsArray.length; a++) {

        var tempElement = document.createElement('tempElement');
        tempElement.innerHTML = fragmentsArray[a].innerHTML;

        var currentRez = GetXpathArray(xpathString, false, tempElement);

        if (currentRez.length > 0) {
            for (var xr = 0; xr < currentRez.length; xr++) {
                var selid = currentRez[xr].getAttribute("selenium-id");
                if (selid != null) {
                    var cssresult = searchForElementByCss('*[selenium-id=\"' + selid + '\"]');
                    cssResults.push(cssresult);
                }
            }
        }
    }
    return cssResults;
}

function setCustomAttributesForAllFields(fragmentsArray) {
    for (var j = 0; j < fragmentsArray.length; j++) {
        if (fragmentsArray[j] != null) {
            if (fragmentsArray[j].children != null) {
                for (var c = 0; c < fragmentsArray[j].children.length; c++) {
                    fragmentsArray[j].children[c].setAttribute("selenium-id", seleniumIdCounter);
                    seleniumIdCounter = seleniumIdCounter + 1;
                    setCustomAttributesForChildElements(fragmentsArray[j].children[c]);

                }
            }
        }
    }
}

function setCustomAttributesForChildElements(element) {
    for (var j = 0; j < element.children.length; j++) {
        if (element.children[j] != null) {
            element.children[j].setAttribute("selenium-id", seleniumIdCounter);
            seleniumIdCounter = seleniumIdCounter + 1;
            setCustomAttributesForChildElements(element.children[j]);
        }
    }
}



function searchForAllElementsByCss(selector, rootElement) {
    var fragmentsArray = getAllShadowDomDocumentFragments(rootElement);

    var cssResultArray = [];
    for (var a = 0; a < fragmentsArray.length; a++) {

        var currentRez = fragmentsArray[a].querySelectorAll(selector);
        if (currentRez != null) {
            for (var cr = 0; cr < currentRez.length; cr++) {
                cssResultArray.push(currentRez[cr]);
            }
        }
    }
    return cssResultArray;
}

function searchForElementByCss(selector, rootElement) {
    var fragmentsArray = getAllShadowDomDocumentFragments(rootElement);

    var cssResultArray = [];

    for (var a = 0; a < fragmentsArray.length; a++) {
        var currentRez = fragmentsArray[a].querySelector(selector);
        if (currentRez != null) {
            cssResultArray.push(currentRez);
        }
    }
    return cssResultArray;
}

function searchForElementById(selector, rootElement) {
    var fragmentsArray = getAllShadowDomDocumentFragments(rootElement);

    var byIdResultArray = [];

    for (var a = 0; a < fragmentsArray.length; a++) {
        var currentRez = fragmentsArray[a].getElementById(selector);
        if (currentRez != null) {
            byIdResultArray.push(currentRez);
        }
    }
    return byIdResultArray;
}



