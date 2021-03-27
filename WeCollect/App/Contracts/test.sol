pragma solidity ^0.8.3;

contract Test {
    int _multiplier;

    event onMultiply(
        int indexed a,
        address indexed sender,
        int result
    );

    constructor() public {
        //_multiplier = multipier;
    }

    function mulitply(int val) public payable returns (int d){
        d = val * _multiplier;
        emit onMultiply(val, msg.sender, d);
        return d;
    }
}
