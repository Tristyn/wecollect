pragma solidity ^0.4.24;

contract test {
    int _multiplier;

    event onMultiply(
        int indexed a,
        address indexed sender,
        int result
    );

    constructor(int multipier) public {
        _multiplier = multipier;
    }

    function mulitply(int val) public payable returns (int d){
        d = val * _multiplier;
        emit onMultiply(val, msg.sender, d);
        return d;
    }
}