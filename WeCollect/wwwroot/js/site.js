let web3Methods = new web3Methods(window.web3, window.contractInfo);









class web3Methods {
    constructor(web3, contractInfo) {
        this.web3 = web3
        this.contractInfo = contractInfo
    }



    buyCard(cardId) {
        this.ensureWeb3Authorized();
    }
    
    ensureWeb3Authorized() {
        if (this.isWeb3Authorized) {
            return;
        }

        authorizeWeb3()
    }

    authorizeWeb3() {
        let body = document.getElementsByTagName("body")[0]
        let modal = document.createElement("div");
        modal.classList.add("metamask-install-modal")
        modal.classList.add("container body-content")
        
        let header = modal.appendChild(document.createElement("h2"))
        header.textContent = "Create your trading wallet"
        let modalContent = modal.appendChild(document.createElement("p"))
        modalContent.textContent = "You'll need a safe place to store your Contracts."
        
    }

    isWeb3Authorized() {
        return this.web3 != undefined
    }
}