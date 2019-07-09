     $.ajax({

        url: "/Home/TestStatictics",
        type: "GET",
        datatype: {},

        success: function (json) {
            DisplayStatictics(json);
        },
        error: function (error) {
        }
    });

function DisplayStatictics(Model)
{
    
    var ID = [,];
   

    for (var i = 0; i < Model.languageDTO.length; i++)
    {
        
        ID[0] = Model.languageDTO[i].languageId;
        var score = CheckIsScore('languageDTO', ID, Model);
        
        if (score > 0)
        { 
            CreateCollaps('languageDTO', 'accordion', `${Model.languageDTO[i].languageId}`, Model.languageDTO[i], 'btn-primary', score);

            for (var j = 0; j < Model.categoriesDTO.length; j++)
            {   
                ID[1] = 0;
                ID[1] = Model.categoriesDTO[j].categoryId;
                
                var score = CheckIsScore('categoriesDTO', ID, Model);

                if (score > 0)
                {
                    CreateCollaps(`categoriesDTO${i}`, `languageDTO${i + 1}`, `${Model.categoriesDTO[j].categoryId}`, Model.categoriesDTO[j], 'btn-warning', score);

                    for (var q = 0; q < Model.testsDTO.length; q++)
                    {
                        ID[2] = 0;
                        ID[2] = Model.testsDTO[q].testId;
                        
                        var score = CheckIsScore('testsDTO', ID, Model);
                        if (score > 0)
                        { 
                            CreateCollaps(`testsDTO${j}`, `categoriesDTO${i}${j + 1}`, `${Model.testsDTO[q].testId}`, Model.testsDTO[q], 'btn-success', score);
                        }
                    }

                }
            }
        }
    }
}




function CreateCollaps(prefix, createPoint, id, model, btnCollor, score)
{
    var createpoint = document.getElementById(`${createPoint}`);
    
    var divCartHeader = document.createElement('div');
    divCartHeader.classList.add('card-header');
    divCartHeader.id = `Heading${prefix}${id}`;

    var h5mb0 = document.createElement('h5');
    h5mb0.classList.add(`mb-0`);

    var btn = document.createElement('button');
    btn.classList.add('btn',`${btnCollor}`);
    btn.setAttribute('data-toggle', 'collapse');
    btn.setAttribute('data-target', `#${prefix}${id}`);
    btn.setAttribute('aria-expanded', 'true');
    btn.setAttribute('aria-controls', `${prefix}${id}`);
    btn.innerText = `${model.translation} (${model.native}) `;

    var span = document.createElement('span');
    span.classList.add('badge', 'badge-pill', 'badge-light');
    span.innerText = `${score}`;

    btn.appendChild(span);

    h5mb0.appendChild(btn);
    divCartHeader.appendChild(h5mb0);

    var divCollaps = document.createElement('div');
    divCollaps.classList.add('collapse');
    divCollaps.id = `${prefix}${id}`
    divCollaps.setAttribute('aria-labelledby', `Heading${prefix}${id}`);
    divCollaps.setAttribute('data-parent', `accordion`);
    

    var divCard = document.createElement('div');
    divCard.classList.add('card');

    h5mb0.appendChild(btn);
    divCard.appendChild(h5mb0);
    

    divCard.appendChild(divCollaps);

    

    createpoint.appendChild(divCard);
}

function CheckIsScore(prefix, id, Model)
{
    var totalscore = 0;

    if (prefix == 'languageDTO')
    {
        for (var i = 0; i < Model.totalScore.length; i++)
        {
            if (id[0] == Model.totalScore[i].langId)
            {
                totalscore = Model.totalScore[i].total;
            }
        }
    }

    if (prefix == 'categoriesDTO')
    {
        for (var i = 0; i < Model.testResults.length; i++)
        {
            if ((id[1] == Model.testResults[i].categoryId) && (id[0] == Model.testResults[i].langId))
            {
                totalscore = totalscore + Model.testResults[i].result;
            }
        }
    }

    if (prefix == 'testsDTO') {
        for (var i = 0; i < Model.testResults.length; i++) {
            if ((id[1] == Model.testResults[i].categoryId) && (id[0] == Model.testResults[i].langId) && (id[2] == Model.testResults[i].testId))
            {
                totalscore = totalscore + Model.testResults[i].result;
            }
        }
    }

 
    
    return totalscore
}