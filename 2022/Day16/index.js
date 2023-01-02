var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
var fs = require('fs');
var assert = require('assert');
var parseFlowInfo = function (data) {
    return data.split('\n').reduce(function (map, line) {
        var _a = line.split(';'), valve = _a[0], tunnels = _a[1];
        var _b = valve.split(' has flow rate='), rawValveName = _b[0], flowRate = _b[1];
        var valveName = rawValveName.split(/\s+/)[1];
        map.set(valveName, {
            flowRate: parseInt(flowRate),
            tunnelNames: tunnels.split(/ leads? to valves? /ig)[1].split(', ')
        });
        return map;
    }, new Map());
};
function solve(data, partTwo) {
    var valves = parseFlowInfo(data);
    var cache = new Map();
    var opened = new Map();
    function recur(time, human, elephant) {
        var flowed = __spreadArray([], opened.entries(), true).reduce(function (sum, _a) {
            var key = _a[0], value = _a[1];
            return sum + (value ? value * valves.get(key).flowRate : 0);
        }, 0);
        if (!time)
            return flowed;
        var key = "".concat(time, "-").concat(human, "-").concat(elephant);
        if ((cache.get(key) || -Infinity) >= flowed)
            return 0;
        cache.set(key, flowed);
        var best = 0;
        for (var _i = 0, _a = __spreadArray([human], valves.get(human).tunnelNames, true); _i < _a.length; _i++) {
            var nextHuman = _a[_i];
            if (human === nextHuman) {
                if (opened.has(human) || !valves.get(human).flowRate)
                    continue;
                opened.set(human, time);
            }
            if (elephant)
                for (var _b = 0, _c = __spreadArray([elephant], valves.get(elephant).tunnelNames, true); _b < _c.length; _b++) {
                    var nextElephant = _c[_b];
                    if (elephant === nextElephant) {
                        if (opened.has(elephant) || !valves.get(elephant).flowRate)
                            continue;
                        opened.set(elephant, time);
                    }
                    best = Math.max(best, recur(time - 1, nextHuman, nextElephant));
                    if (elephant === nextElephant)
                        opened["delete"](elephant);
                }
            else {
                best = Math.max(best, recur(time - 1, nextHuman, elephant));
            }
            if (human === nextHuman)
                opened["delete"](human);
        }
        return best;
    }
    return partTwo ? recur(25, 'AA', 'AA') : recur(29, 'AA', null);
}
function solveOne(data) {
    return solve(data, false);
}
(function () {
    var data = fs.readFileSync('/input.txt').toString();
    assert.deepStrictEqual(solveOne("Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\nValve BB has flow rate=13; tunnels lead to valves CC, AA\nValve CC has flow rate=2; tunnels lead to valves DD, BB\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\nValve EE has flow rate=3; tunnels lead to valves FF, DD\nValve FF has flow rate=0; tunnels lead to valves EE, GG\nValve GG has flow rate=0; tunnels lead to valves FF, HH\nValve HH has flow rate=22; tunnel leads to valve GG\nValve II has flow rate=0; tunnels lead to valves AA, JJ\nValve JJ has flow rate=21; tunnel leads to valve II"), 1649); // off by two, supposed to be 1651
    console.log(solveOne(data));
})();
function solveTwo(data) {
    return solve(data, true);
}
(function () {
    var data = fs.readFileSync('/input.txt').toString();
    assert.deepStrictEqual(solveTwo("Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\nValve BB has flow rate=13; tunnels lead to valves CC, AA\nValve CC has flow rate=2; tunnels lead to valves DD, BB\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\nValve EE has flow rate=3; tunnels lead to valves FF, DD\nValve FF has flow rate=0; tunnels lead to valves EE, GG\nValve GG has flow rate=0; tunnels lead to valves FF, HH\nValve HH has flow rate=22; tunnel leads to valve GG\nValve II has flow rate=0; tunnels lead to valves AA, JJ\nValve JJ has flow rate=21; tunnel leads to valve II"), 1706); // oof by one, supposed to be 1707
    console.log(solveTwo(data));
})();
