var beneficiarios = [];
var indiceEdicao = -1;

$(document).ready(function () {
    $('#CPFBeneficiario').inputmask('999.999.999-99');

    if (typeof obj !== 'undefined' && obj && obj.Beneficiarios) {
        beneficiarios = obj.Beneficiarios;
    }
    listarBeneficiarios();

    $('#btnIncluirBeneficiario').click(function () {
        var cpf = $('#CPFBeneficiario').val();
        var nome = $.trim($('#NomeBeneficiario').val());

        if (!validarCPF(cpf)) {
            ModalDialog("Ocorreu um erro", "CPF do beneficiário inválido.");
            return;
        }
        if (nome === '') {
            ModalDialog("Ocorreu um erro", "Informe o nome do beneficiário.");
            return;
        }

        var Cpfduplicado = beneficiarios.some(function (b, i) {
            return b.CPF === cpf && i !== indiceEdicao;
        });
        if (Cpfduplicado) {
            ModalDialog("Ocorreu um erro", "Já existe um beneficiário com este CPF para este cliente.");
            return;
        }

        if (indiceEdicao >= 0) {
            beneficiarios[indiceEdicao].CPF = cpf;
            beneficiarios[indiceEdicao].Nome = nome;
        } else {
            beneficiarios.push({ Id: 0, CPF: cpf, Nome: nome });
        }

        limparCamposBeneficiario();
        listarBeneficiarios();
    });
});

function listarBeneficiarios() {
    var grid = $('#gridBeneficiarios').empty();

    $.each(beneficiarios, function (i, b) {
        var btnAlterar = $('<button type="button" class="btn btn-sm btn-primary">Alterar</button>')
            .click(function () { editarBeneficiario(i); });
        var btnExcluir = $('<button type="button" class="btn btn-sm btn-primary">Excluir</button>')
            .click(function () { excluirBeneficiario(i); });

        $('<tr>')
            .append($('<td>').text(b.CPF))
            .append($('<td>').text(b.Nome))
            .append($('<td>').append(btnAlterar, ' ', btnExcluir))
            .appendTo(grid);
    });
}

function editarBeneficiario(i) {
    indiceEdicao = i;
    $('#CPFBeneficiario').val(beneficiarios[i].CPF);
    $('#NomeBeneficiario').val(beneficiarios[i].Nome);
    $('#btnIncluirBeneficiario').text('Alterar');
}

function excluirBeneficiario(i) {
    beneficiarios.splice(i, 1);
    limparCamposBeneficiario();
    listarBeneficiarios();
}

function limparCamposBeneficiario() {
    indiceEdicao = -1;
    $('#CPFBeneficiario').val('');
    $('#NomeBeneficiario').val('');
    $('#btnIncluirBeneficiario').text('Incluir');
}

function limparBeneficiarios() {
    beneficiarios = [];
    limparCamposBeneficiario();
    listarBeneficiarios();
}

function adicionarBeneficiariosAoEnvio(dados) {
    $.each(beneficiarios, function (i, b) {
        dados['Beneficiarios[' + i + '].Id'] = b.Id;
        dados['Beneficiarios[' + i + '].CPF'] = b.CPF;
        dados['Beneficiarios[' + i + '].Nome'] = b.Nome;
    });
    return dados;
}

function validarCPF(cpf) {
    cpf = (cpf || '').replace(/\D/g, '');
    if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf))
        return false;

    var soma = 0;
    for (var i = 0; i < 9; i++) soma += parseInt(cpf[i]) * (10 - i);
    var resto = soma % 11;
    var digito1 = resto < 2 ? 0 : 11 - resto;

    soma = 0;
    for (var j = 0; j < 10; j++) soma += parseInt(cpf[j]) * (11 - j);
    resto = soma % 11;
    var digito2 = resto < 2 ? 0 : 11 - resto;

    return parseInt(cpf[9]) === digito1 && parseInt(cpf[10]) === digito2;
}