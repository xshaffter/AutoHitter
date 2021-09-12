let table, split_displays, split, split_count, run_data_frame, foft, run_number;

function doUpdateFrame() {
    try {
        run_data_frame.setAttribute("src", "run_data.html")
        Watchdog();
    } catch {

    }
}

var heartbeat = 0;
var init_done = false;
function Watchdog() {
    setTimeout(function () { Watchdog(); }, 100); // refresh every second
    if (heartbeat <= 1)
        heartbeat++;
    else {
        if (!init_done) // reading the data file never worked before or updating failed?
        {
            ShowHelpText('The browser or broadcasting software cannot read the hit counter data file.');
        }
        run_data_frame.src = './run_data.html'; // retry reloading file in case of errors
    }
}
var last_update_time = 0;
function DoUpdate(total, splits, run_data) {
    heartbeat = 0; // reset heartbeat, because we are alive
    init_done = true; // the data file could be loaded successfully
    //ShowHelpText(JSON.stringify(splits))
    doUpdateData(total, splits, run_data); // build graphical content
    setTimeout(function () { iframe.src = './run_data.html'; }, 100); // refresh around every second
}

function ShowHelpText(instructions) {
    table.innerHTML = '<tr><td class="major dark">' + instructions + '</td></tr>';
}

function doUpdateData(total_info, splits, run_data) {
    try {
        table.style = "";
        split.textContent = run_data.split + 1;
        split_count.textContent = run_data.split_count;
        run_number.textContent = run_data.run;
        if (run_data.fury) {
            foft.classList = foft.classList.remove("hidden");
        } else {
            foft.classList.add("hidden");
        }
        while (split_displays.firstChild) {
            split_displays.removeChild(split_displays.firstChild);
        }
        splits.forEach((split, index) => {
            let row = document.createElement("tr")
            if (split.split == run_data.split) {
                row.classList.add("current-split");
            } else if (split.split < run_data.split) {
                if (split.Diff < 0) {
                    row.classList.add("bad-split");
                } else {
                    row.classList.add("good-split");
                }
            }

            let name = document.createElement("td");
            let now = document.createElement("td");
            let diff = document.createElement("td");
            let pb = document.createElement("td");

            name.innerHTML = split.Name;
            now.innerHTML = split.Hits;
            diff.innerHTML = split.Diff > 0 ? `+${split.Diff}` : split.Diff;
            pb.innerHTML = split.PB;

            row.appendChild(name);
            row.appendChild(now);
            row.appendChild(diff);
            row.appendChild(pb);

            split_displays.appendChild(row);
        })
        loadTotals(total_info);
    } catch (ex) {
        ShowHelpText(ex)
    }
}

function loadTotals(total) {
    let row = document.createElement("tr")

    row.classList.add("total");

    let name = document.createElement("td");
    let now = document.createElement("td");
    let diff = document.createElement("td");
    let pb = document.createElement("td");

    name.innerHTML = total.Name;
    now.innerHTML = total.Hits;
    diff.innerHTML = total.Diff > 0 ? `+${total.Diff}` : total.Diff;
    pb.innerHTML = total.PB;


    row.appendChild(name);
    row.appendChild(now);
    row.appendChild(diff);
    row.appendChild(pb);

    split_displays.appendChild(row);
}


window.onload = function () {
    table = document.getElementById("hit-counter");
    split_displays = document.getElementById("splits");
    split = document.getElementById("split");
    split_count = document.getElementById("split_count");
    foft = document.getElementById("foft");
    run_number = document.getElementById("run_count");
    run_data_frame = document.getElementById("run_data_frame");
    doUpdateFrame();
}